from receiver_socket import ReceiverSocket
from payload_handler import PayloadHandler

def main():
    print("=========== Receiver ==========")

    receiver_socket = ReceiverSocket()

    broker_ip = "127.0.0.1"
    broker_port = 9090

    receiver_socket.connect(broker_ip, broker_port)

    authenticated = False

    while not authenticated:
        print()
        print("Sing up or sign in? ( 1 = sign up, 2 = sign in): ", end="")

        choice = input()

        if choice not in ("1", "2"):
            print("Invalid option. Choose 1 or 2")
            continue

        username = input("Username: ")
        password = input("Password: ")

        success, message = receiver_socket.authenticate(
            username,
            password,
            is_sign_up=(choice == "1")
        )

        if success:
            authenticated = True
            print(message)
        else:
            print(f"Autenthication failed: {message}. Try again.")


    running = True
    while running:
        print()
        print("========== MENU ==========")
        print("1. Subscribe to topic")
        print("2. View subscribed topics")
        print("3. View received messages")
        print("4. Exit")
        print("==========================")

        option = input("Choose an option: ")

        if option == "1":
            topic = input("Enter topic: ").lower()

            if topic.strip():
                receiver_socket.subscribe(topic)

        elif option == "2":
            print()
            print("----- SUBSCRIBED TOPICS -----")

            topics = receiver_socket.get_topics()

            if len(topics) == 0:
                print("No subscribed topics")
            else:
                for topic in topics:
                    print(f"- {topic}")

            print("-----------------------------")

        elif option == "3":
            print()
            print("===== RECEIVED MESSAGES ======")

            messages = PayloadHandler.get_messages()

            if len(messages) == 0:
                print("No messages received.")
            else:
                for topic, topic_messages in messages.items():
                    print()
                    print(f"[{topic}]")

                    for message in topic_messages:
                        print(f"- {message}")

            print("==============================")

        elif option == "4":
            running = False

        else:
            print("Invalid option.")


if __name__ == "__main__":
    main()

