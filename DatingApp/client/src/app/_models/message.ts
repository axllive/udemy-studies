export interface Message {
    id: number
    senderid: number
    senderusername: string
    senderphotourl: string
    recipientid: number
    recipientusername: string
    recipientphotourl: string
    content: string
    dateread?: Date
    messagesent: Date
  }
  