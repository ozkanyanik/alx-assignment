import { Injectable } from '@angular/core';
import * as appConfig from '../../assets/web-config.json'

@Injectable()
export class ConfigurationService {

    constructor() {
    }
    
    getApiEndpoint(): string {
        return ((<any>appConfig).apiEndpoint);
    }
}