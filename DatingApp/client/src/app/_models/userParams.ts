import {User} from "./user";

export class UserParams{
    gender: string;
    minAge = 18;
    maxAge = 99;
    pageNumber = 1;
    pageSize = 12;
    orderBy = 'lastActive';
    /**
     *
     */
    constructor(user: User) {
        if(user.gender == 'male')
            this.gender = 'female';
        else if (user.gender == 'female')
            this.gender = 'male';
        else
            this.gender = 'other';
    }
}