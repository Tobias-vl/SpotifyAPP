import {
  HubConnection,
  HubConnectionBuilder,
  HubConnectionState,
  LogLevel,
} from "@microsoft/signalr";

import React, { createContext, useContext, useEffect, useRef, useState} from 'react';
let connection: HubConnection | null = null;

export const SignalRContext = createContext(null);
export const SignalRProvider = ({children, serverUrl, accessToken, getAccessToken }) => {
    const [hub, setHub] = useState(null);
    useState(() => {
        if (!serverUrl) {
            setHub(null);
            return;
        }
    })
}
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

export async function ensureConnected() {
  const hubConnection = getConnection();

  if (hubConnection.state === HubConnectionState.Disconnected) {
    await hubConnection.start();
  }

  return hubConnection;
}
