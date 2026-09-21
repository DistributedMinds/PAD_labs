import socket
import threading


class SenderSocket:
    def __init__(self):
        self._socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.is_connected = False

    def connect(self, ip_address: str, port: int):
        thread = threading.Thread(target=self._connect_callback, args=(ip_address, port))
        thread.start()

        thread.join(timeout=2)

    def _connect_callback(self, ip_address: str, port: int):
        try:
            self._socket.connect((ip_address, port))
            self.is_connected = True
            print("Sender connected to brocker")
        except OSError as e:
            self.is_connected = False
            print(f"Error: Sender could not connect to brocker ({e})")

    def send(self, data: bytes):
        try:
            self._socket.sendall(data)
        except OSError as e:
            print(f"Could not send data {e}")