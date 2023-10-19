export interface User{
    username: string;
    bio: string;
    token: string;
    photos: {
        url: string,
        ismain: boolean
    }[];
}