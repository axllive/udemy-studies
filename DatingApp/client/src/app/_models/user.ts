export interface User{
    username: string;
    gender: string;
    knownas: string;
    bio: string;
    token: string;
    photos: {
        url: string,
        ismain: boolean
    }[];
}