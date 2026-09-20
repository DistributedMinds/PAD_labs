import socket
import threading

from payload_handler import PayloadHandler

class ReceiverSocket:
    def __init__(self):
        self._socket = socket.socket(
            socket.AF_INET,
            socket.SOCK_STREAM
        )

        self._topics = []

    def connect(self, ip_address, port):
        self._socket.connect((ip_address, port))
        print("Receiever connected to broker")

    def authenticate(self, username, password, is_sign_up):
        command = "signup" if is_sign_up else "signin"

        data = f"{command}#{username}#{password}".encode("utf-8")

        self._socket.send(data)

        response = self._socket.recv(1024).decode("utf-8")

        parts = response.split("#")

        if parts[0] == "authOK":
            if len(parts) > 1 and parts[1]:
                for topic in parts[1].split(","):
                    if topic not in self._topics:
                        self._topics.append(topic)

            self.start_receive()

            return True, "Authenticated succcessfully."

        reason = parts[1] if len(parts) > 1 else "unknown error"

        return False, reason

    def subscribe(self, topic):
        if topic in self._topics:
            print(f"Already subscribed to: {topic}")
            return

        self._topics.append(topic)

        data = f"subscribe#{topic}".encode("utf-8")

        self.send(data)

        print(f"Subscribed to: {topic}")

    def get_topics(self):
        return self._topics

    def start_receive(self):
        receive_thread = threading.Thread(
            target = self.receive_messages,
            daemon=True
        )

        receive_thread.start()

    def receive_messages(self):
        while True:
            try:
                payload_bytes = self._socket.recv(4096)

                if not payload_bytes:
                    break

                PayloadHandler.handle(payload_bytes)

            except Exception as e:
                print(f"Can't receive data from broker. {e}")
                break

    def send(self, data):
        try:
            self._socket.send(data)
        except Exception as e:
            print(f"Could not send data: {e}")