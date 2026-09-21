import json
import xml.etree.ElementTree as ET

from Sender_socket import SenderSocket


class Settings:
    broker_ip = "127.0.0.1"
    broker_port = 9090


class PayLoad:
    def __init__(self, topic: str = "", message: str = ""):
        self.topic = topic
        self.message = message

    def to_json(self) -> str:
        return json.dumps({"Topic": self.topic, "Message": self.message})

    def to_xml(self) -> str:
        root = ET.Element("PayLoad")
        topic_el = ET.SubElement(root, "Topic")
        topic_el.text = self.topic
        message_el = ET.SubElement(root, "Message")
        message_el.text = self.message
        return ET.tostring(root, encoding="unicode")


def main():
    print("Sender")

    sender_socket = SenderSocket()
    sender_socket.connect(Settings.broker_ip, Settings.broker_port)

    if sender_socket.is_connected:
        while True:
            pay_load = PayLoad()

            pay_load.topic = input("Enter topic: ").lower()
            pay_load.message = input("Enter message: ")

            format_choice = ""
            while format_choice not in ("1", "2"):
                format_choice = input("Choose serialization format (1 for JSON, 2 for XML): ")
                if format_choice not in ("1", "2"):
                    print("Invalid choice. Choose again")

            if format_choice == "1":
                pay_load_string = pay_load.to_json()
            else:
                pay_load_string = pay_load.to_xml()

            data = pay_load_string.encode("utf-8")
            sender_socket.send(data)
    else:
        input()


if __name__ == "__main__":
    main()