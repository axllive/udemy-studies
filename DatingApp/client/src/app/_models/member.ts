import { Photo } from "./photo"

export interface Member {
    username: string
    password: string
    gender: string
    bio: string
    age: number
    kwonas: string
    created: Date
    lastactive: Date
    intrests: string
    lookingfor: string
    city: string
    country: string
    photos: Photo[]
  }
  
