"use client";

import { useEffect, useState } from "react";
import { ensureConnected, getConnection } from "@/lib/socket";

type ServerMessage = {
  text: string;
};

export function useSocketMessages() {
  const [connected, setConnected] = useState(false);
  const [messages, setMessages] = useState<string[]>([]);

  useEffect(() => {
    const connection = getConnection();

    const onConnect = () => {
      setConnected(true);
      setMessages((prev) => [...prev, "Connected to SignalR hub"]);
    };

    const onDisconnect = () => {
      setConnected(false);
      setMessages((prev) => [...prev, "Disconnected from SignalR hub"]);
    };

    const onServerMessage = (payload: ServerMessage | string) => {
      const text =
        typeof payload === "string" ? payload : payload.text ?? "(empty payload)";
      setMessages((prev) => [...prev, `Server: ${text}`]);
    };

    const onConnectError = (error: unknown) => {
      const message = error instanceof Error ? error.message : "Unknown error";
      setMessages((prev) => [...prev, `Connection error: ${message}`]);
    };

    connection.onreconnected(onConnect);
    connection.onclose(onDisconnect);
    connection.on("ReceiveMessage", onServerMessage);

    ensureConnected()
      .then(() => {
        onConnect();
      })
      .catch(onConnectError);

    return () => {
      connection.off("ReceiveMessage", onServerMessage);
    };
  }, []);

  const sendMessage = (text: string) => {
    if (!text.trim()) {
      return;
    }

    ensureConnected()
      .then((connection) => connection.invoke("SendMessage", text))
      .catch((error: unknown) => {
        const message = error instanceof Error ? error.message : "Unknown error";
        setMessages((prev) => [...prev, `Send failed: ${message}`]);
      });

    setMessages((prev) => [...prev, `You: ${text}`]);
  };

  return {
    connected,
    messages,
    sendMessage,
  };
}
