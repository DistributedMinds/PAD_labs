import json
import threading

class PayloadHandler:
    _messages = {}
    _lock = threading.Lock()

    @staticmethod
    def handle(payload_bytes):
        payload_string = payload_bytes.decode("utf-8")

        try:
            payload = json.loads(payload_string)
        except json.JSONDecodeError:
            return

        if payload is None:
            return

        topic = payload.get("Topic")
        message = payload.get("Message")

        if topic is None or message is None:
            return

        with PayloadHandler._lock:
            if topic not in PayloadHandler._messages:
                PayloadHandler._messages[topic] = []

            PayloadHandler._messages[topic].append(message)

    @staticmethod
    def get_messages():
        with PayloadHandler._lock:
            return {
                topic: messages.copy()
                for topic, messages in PayloadHandler._messages.items() 
            }