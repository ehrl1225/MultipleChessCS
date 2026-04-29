import logging
from typing import Callable

from signalrcore.hub_connection_builder import HubConnectionBuilder
from signalrcore.hub.base_hub_connection import BaseHubConnection

from client.chess_hub_interface import IChessHub
from client.request_enum import RequestEnum


class SignalRClient(IChessHub):

    def __init__(self, url):
        self.url = url
        self.connection: BaseHubConnection = (
            HubConnectionBuilder()
            .with_url(self.url)
            .configure_logging(logging.DEBUG)
            .with_automatic_reconnect({
                "type": "raw",
                "keep_alive_interval" : 10,
                "reconnect_interval" : 5,
                "max_attempts" : 5
            })
            .build()
        )
        self.is_open = False
        self.connection.on_open(self.onOpen)
        self.connection.on_close(self.onClose)

        self.on_register_response = None
        self.on_login_response = None

    def onOpen(self):
        self.is_open = True
        print("Connection opened")

    def onClose(self):
        self.is_open = False
        print("Connection closed")

    def addHandler(self, event_name: str, handler: Callable):
        self.connection.on(event_name, handler)

    def connect(self):
        self.connection.start()

    def send(self, method: str, args: list):
        if self.is_open:
            self.connection.send(method, args)

    def request_login(self, username, password):
        self.send(RequestEnum.RequestLogin.value, [username, password])

    def request_register(self, username: str, password: str):
        pass


