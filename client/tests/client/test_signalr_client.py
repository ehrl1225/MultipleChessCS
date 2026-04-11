import pytest
from client.signalr_client import SignalRClient
from tests.common.common import URL

def test_signalr_client():
    client = SignalRClient(URL)
    