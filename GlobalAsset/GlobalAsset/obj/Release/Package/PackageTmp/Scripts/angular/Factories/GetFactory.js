var GetFactory = function ($http, $q) {
    return function (url, data) {
        var deferredObject = $q.defer();
        //debugger;
        if (typeof (data) !== "undefined") {
            $http.get(
             baseUrl + url, data
            ).
            success(function (data) {
                //debugger;
                if (data != null) {
                    deferredObject.resolve({ success: true, data: data });
                } else {
                    deferredObject.resolve({ success: false });
                }
            }).
            error(function () {
                deferredObject.resolve({ success: false });
            });
        } else {
            $http.get(
             baseUrl + url
            ).
            success(function (data) {
                //debugger;
                if (data != null) {
                    deferredObject.resolve({ success: true, data: data });
                } else {
                    deferredObject.resolve({ success: false });
                }
            }).
            error(function () {
                deferredObject.resolve({ success: false });
            });
        }
        

        return deferredObject.promise;
    }
}

GetFactory.$inject = ['$http', '$q'];