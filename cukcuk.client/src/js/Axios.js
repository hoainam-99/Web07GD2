import axios from "axios";

// import axios from "axios";
var Axios = Axios || {};

Axios.Methods = {
    Get: "GET",
    Post: "POST",
    Put: "PUT",
    Delete: "DELETE",
}

Axios.Domain = "http://localhost:5150/";

Axios.Url = {
    Material: `${Axios.Domain}api/v1/Materials`,
    FilterMaterial: `${Axios.Domain}api/v1/Materials/Filter`,
    NewMaterialCode: `${Axios.Domain}api/v1/Materials/new-code`,
    Stock: `${Axios.Domain}api/v1/Stocks`,
    Category: `${Axios.Domain}api/v1/Categories`,
    Unit: `${Axios.Domain}api/v1/Units`,
}

Axios.CallAxios = (method, url, data, resType) => {
    if(!resType){
        resType = 'json';
    }
    return axios({
        method: method,
        url: url,
        data: data,
        responseType: resType
    })
}

export default Axios;