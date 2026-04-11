import logging
from signalrcore.hub_connection_builder import HubConnectionBuilder
from signalrcore.hub.base_hub_connection import BaseHubConnection

class SignalRClient:

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
        self.connection.on_open(lambda : print("Connection opened"))
        self.connection.on_close(lambda : print("Connection closed"))

        self.on_register_response = None
        self.on_login_response = None

    def _setup_handler(self):
        self.connection.on("Login", self._handle_login_response)

    def _handle_login_response(self, args):
        pass


