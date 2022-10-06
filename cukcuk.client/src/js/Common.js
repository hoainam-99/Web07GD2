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
        
        if(diffYear != 0){
            returnValue = {
                dateType: Resource.DateType.Year,
                dateValue: diffYear
            }
        }else if(diffMonth != 0){
            returnValue = {
                dateType: Resource.DateType.Month,
                dateValue: diffMonth
            }
        }else{
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

export default CommonFn;