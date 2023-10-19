import { Photo } from "./photo"

export interface Root {
    username: string
    password: string
    gender: string
    bio: string
    age: number
    kwonas: string
    created: Date
    lastactive: Date
    intrests: string
    city: string
    country: string
    photos: Photo[]
  }
  
