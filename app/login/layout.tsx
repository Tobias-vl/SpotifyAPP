import * as React from "react"
import { useState } from "react"
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card"
import { Button } from "@/components/ui/button"
import { Avatar, AvatarFallback } from "@/components/ui/avatar"
import { Github } from "lucide-react"

export default function SocialAccountLinkingCard() {
  type Account = {
    name: string
    connected: boolean
    icon: React.ReactNode
  }

  const [accounts, setAccounts] = useState<Account[]>([
    { name: "Google", connected: true, icon: <Avatar className="h-5 w-5 bg-red-500"><AvatarFallback>G</AvatarFallback></Avatar> },
    { name: "GitHub", connected: false, icon: <Github className="h-5 w-5" /> },
    { name: "Facebook", connected: false, icon: <Avatar className="h-5 w-5 bg-blue-500"><AvatarFallback>F</AvatarFallback></Avatar> },
  ])

  const toggleConnection = (index: number) => {
    setAccounts((prev) => {
      const updated = prev.map((acc, i) => i === index ? { ...acc, connected: !acc.connected } : acc)
      return updated
    })
  }

  return (
    <Card className="max-w-md mx-auto shadow-md">
      <CardHeader>
        <CardTitle>Linked Accounts</CardTitle>
        <CardDescription>Manage your connected social accounts</CardDescription>
      </CardHeader>
      <CardContent className="space-y-4">
        {accounts.map((account, index) => (
          <div
            key={account.name}
            className="flex items-center justify-between rounded-md border border-border p-3"
          >
            <div className="flex items-center gap-3">
              {account.icon}
              <span className="font-medium">{account.name}</span>
            </div>
            <Button
              size="sm"
              variant={account.connected ? "destructive" : "outline"}
              onClick={() => toggleConnection(index)}
            >
              {account.connected ? "Disconnect" : "Connect"}
            </Button>
          </div>
        ))}
      </CardContent>
    </Card>
  )
}