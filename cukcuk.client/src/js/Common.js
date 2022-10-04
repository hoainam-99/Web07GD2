import Resource from "./Resource.js";

var CommonFn = CommonFn || {};

CommonFn.formatInputExpiryDate = (date) => {
    let returnValue = {
        dateType: Resource.DateType.Date,
        dateValue: 0
    }
    if (date) {
        let expDate = new Date(date),
            today = new Date();
        if (expDate.getFullYear() != today.getFullYear()) {
            returnValue = {
                dateType: Resource.DateType.Year,
                dateValue: expDate.getFullYear() - today.getFullYear()
            }

            return returnValue;
        }

        if (expDate.getMonth() != today.getMonth()) {
            returnValue = {
                dateType: Resource.DateType.Month,
                dateValue: expDate.getMonth() - today.getMonth()
            }

            return returnValue;
        }

        if (expDate.getDate() != today.getDate()) {
            returnValue = {
                dateType: Resource.DateType.Date,
                dateValue: expDate.getDate() - today.getDate()
            }

            return returnValue;
        }
    }

    return returnValue;
}

CommonFn.formatOutputExpiryDate = (expDate) => {
    if (expDate) {
        let newExpDate = new Date(),
            day, month, year;
        switch (expDate.dateType) {
            case Resource.DateType.Date:
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