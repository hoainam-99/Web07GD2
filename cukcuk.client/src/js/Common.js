import Resource from "./Resource.js";

var CommonFn = CommonFn || {};

CommonFn.formatInputExpiryDate = (date) => {
    let returnValue = {
        dateType: Resource.DateType.Day,
        dateValue: 0
    }
    if (date) {
        let expDate = new Date(date),
            today = new Date(),
            diff = new Date(expDate.getTime() - today.getTime()),
            diffYear = diff.getFullYear() - 1970,
            diffMonth = diff.getMonth(),
            diffDay = diff.getDate();

        if (diffYear != 0) {
            returnValue = {
                dateType: Resource.DateType.Year,
                dateValue: diffYear
            }
        } else if (diffMonth != 0) {
            returnValue = {
                dateType: Resource.DateType.Month,
                dateValue: diffMonth
            }
        } else {
            returnValue = {
                dateType: Resource.DateType.Day,
                dateValue: diffDay
            }
        }
    }

    return returnValue;
}

CommonFn.formatOutputExpiryDate = (expDate) => {
    if (expDate) {
        let newExpDate = new Date(),
            day, month, year;
        expDate.dateValue = parseInt(expDate.dateValue);
        switch (expDate.dateType) {
            case Resource.DateType.Day:
                newExpDate.setDate(newExpDate.getDate() + expDate.dateValue);
                break;
            case Resource.DateType.Month:
                newExpDate.setMonth(newExpDate.getMonth() + expDate.dateValue);
                break;
            case Resource.DateType.Year:
                newExpDate.setFullYear(newExpDate.getFullYear() + expDate.dateValue);
                break;
            default:
                break;
        }

        year = newExpDate.getFullYear().toString();
        month = (newExpDate.getMonth() + 1).toString().padStart(2, '0');
        day = newExpDate.getDate().toString().padStart(2, '0');
        return `${year}-${month}-${day}T00:00:00`;
    }
}

CommonFn.getError = (e) => {
    let errors = [],
        resError;
    if (e) {
        switch (e.status) {
            case 400:
                resError = e.data.userMsg.error;
                resError.forEach(item => {
                    errors.push(Resource.ErrorMes[item]);
                });
                break;
            case 404:
                errors.push(Resource.ErrorMes.notFound_Error);
                break;
            case 500:
                errors.push(Resource.ErrorMes.generate_Error)
                break;
        }

        return errors;
    }
}

CommonFn.formatNumber = num => {
    if (num) {
        return parseFloat(num);
    }
}

CommonFn.formatCode = (name) => {
    let code;

    if (name) {
        code = name.split(" ").filter(item => {
            return item != "";
        }).map(item => {
            return CommonFn.removeVietnameseTones(item[0]).toUpperCase();
        }).join("");

        return code;
    }

}

CommonFn.removeVietnameseTones = (str) => {
    str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
    str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
    str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
    str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
    str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
    str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
    str = str.replace(/đ/g, "d");
    str = str.replace(/À|Á|Ạ|Ả|Ã|Â|Ầ|Ấ|Ậ|Ẩ|Ẫ|Ă|Ằ|Ắ|Ặ|Ẳ|Ẵ/g, "A");
    str = str.replace(/È|É|Ẹ|Ẻ|Ẽ|Ê|Ề|Ế|Ệ|Ể|Ễ/g, "E");
    str = str.replace(/Ì|Í|Ị|Ỉ|Ĩ/g, "I");
    str = str.replace(/Ò|Ó|Ọ|Ỏ|Õ|Ô|Ồ|Ố|Ộ|Ổ|Ỗ|Ơ|Ờ|Ớ|Ợ|Ở|Ỡ/g, "O");
    str = str.replace(/Ù|Ú|Ụ|Ủ|Ũ|Ư|Ừ|Ứ|Ự|Ử|Ữ/g, "U");
    str = str.replace(/Ỳ|Ý|Ỵ|Ỷ|Ỹ/g, "Y");
    str = str.replace(/Đ/g, "D");
    // Some system encode vietnamese combining accent as individual utf-8 characters
    // Một vài bộ encode coi các dấu mũ, dấu chữ như một kí tự riêng biệt nên thêm hai dòng này
    str = str.replace(/\u0300|\u0301|\u0303|\u0309|\u0323/g, ""); // ̀ ́ ̃ ̉ ̣  huyền, sắc, ngã, hỏi, nặng
    str = str.replace(/\u02C6|\u0306|\u031B/g, ""); // ˆ ̆ ̛  Â, Ê, Ă, Ơ, Ư
    // Remove extra spaces
    // Bỏ các khoảng trắng liền nhau
    str = str.replace(/ + /g, " ");
    str = str.trim();
    // Remove punctuations
    // Bỏ dấu câu, kí tự đặc biệt
    str = str.replace(
        /!|@|%|\^|\*|\(|\)|\+|\=|\<|\>|\?|\/|,|\.|\:|\;|\'|\"|\&|\#|\[|\]|~|\$|_|`|-|{|}|\||\\/g,
        " "
    );
    return str;
}

export default CommonFn;