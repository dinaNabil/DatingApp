export interface Message {
    id: number;
    senderId: number;
    senderKnownAs: string;
    senderPhtotoUrl: string;
    recipientId: number;
    recipientKnowAs: string;
    recipientPhtotoUrl: string;
    content: string;
    isRead: boolean;
    dateRead: Date;
    messageSent: Date;
}
