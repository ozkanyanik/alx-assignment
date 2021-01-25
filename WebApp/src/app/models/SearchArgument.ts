export class SearchArgument
{
    constructor(){
        this.name =  "";
        this.index = 0;
        this.count = this.index + 5;
    }
    name: string;
    index: number;
    count: number;
}