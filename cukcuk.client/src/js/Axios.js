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
    FilterMaterial: `${Axios.Domain}api/v1/Materials/Filter`
}

Axios.CallAxios = (method, url, resType) => {
    if(!resType){
        resType = 'json';
    }
    return axios({
        method: method,
        url: url,
        responseType: resType,
    })
}

export default Axios;