var PostFactory = function ($http, $q) {
    return function (url, data, isFormData) {
        var deferredObject = $q.defer();
        debugger;
        if (typeof (data) !== "undefined") {
            $http.post(
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
            $http.post(
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

PostFactory.$inject = ['$http', '$q'];