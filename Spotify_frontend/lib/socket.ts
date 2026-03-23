"use client";

import {
  HubConnection,
  HubConnectionBuilder,
  HubConnectionState,
  LogLevel,
} from "@microsoft/signalr";

let connection: HubConnection | null = null;

export function getConnection(): HubConnection {
  if (!connection) {
    connection = new HubConnectionBuilder()
      .withUrl(process.env.NEXT_PUBLIC_SIGNALR_URL ?? "http://localhost:5058/hubs/chat")
      .withAutomaticReconnect([0, 2000, 10000, 30000])
      .configureLogging(LogLevel.Information)
      .build();
  }

  return connection;
}

export async function ensureConnected(): Promise<HubConnection> {
  const hubConnection = getConnection();

  if (hubConnection.state === HubConnectionState.Disconnected) {
    await hubConnection.start();
  }

  return hubConnection;
}
