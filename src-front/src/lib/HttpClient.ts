const qs = require('qs');

import Vue from 'vue';

export class HttpClient {

    constructor(private axios: any) {
        axios.defaults.baseURL =  process.env.VUE_APP_API_HOST;
    }


    get(url, data) {
        return this.axios.get(url, data)
            .catch(error => this.handleResponseError(error))
            .then((response) => {
                return this.processResponse(response, url)
            });
    }



    // обработка ошибки ответа от сервера
    handleResponseError(error: any) {
        throw error;
    }

    private processResponse(response, url) {
        if (response == null) {
            throw new Error(url + "\r\n" + "Нет ответа от сервера");
        }

        if (!response.hasOwnProperty("data")) {
            throw new Error(url + "\r\n" + "Ошибка обработки ответа от сервера. Ответ не имеет свойства data \r\n" + JSON.stringify(response));
        }

        let responseData = response.data;
        if (!responseData.hasOwnProperty("result")) {
            throw new Error(url + "\r\n" + "Ошибка обработки ответа от сервера. Данные в ответе не имеют свойства result \r\n" + JSON.stringify(responseData));
        }
        if (!responseData.hasOwnProperty("data")) {
            throw new Error(url + "\r\n" + "Ошибка обработки ответа от сервера. Данные в ответе не имеют свойства data \r\n" + JSON.stringify(responseData));
        }

        if (responseData.result == "success") {
            return responseData.data;
        }
        else if (responseData.result == "error") {
            throw new Error(responseData.data as string);
        }
        else if (responseData.result == "warning") {
            throw new Error(responseData.data);
        }

        throw new Error(url + "\r\n" + "Неизвестный тип ответа от сервера в поле result: " + JSON.stringify(responseData.result));
    }
}
