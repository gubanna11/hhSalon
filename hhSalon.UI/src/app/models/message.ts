export interface Message{
    fromId: string;
    fromUser: any;
    toId: string;
    toUser:any;
    content: string;
    date: Date;
    isRead: boolean;
}