export class ChatItem{
    userId?: string;
    user?: any;
    messageUnreadCount: number = 0;
    lastMessage?: string;
    isRead?: boolean = false;
    date!: Date;

    toUserId?: string;
    toUser?: any;
}