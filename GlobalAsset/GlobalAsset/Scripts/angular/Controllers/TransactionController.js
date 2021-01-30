var TransactionController = function ($scope, $location, Pagination, GetFactory, PostFactory) {
    $scope.userID = Number($('#hdnUserID').val());
    $scope.TransactionData = [];
    $scope.pagination = Pagination.getNew(10);

    //debugger;

    $scope.GetAllTransactionsByUser = function () {
        if ($scope.userID != 0) {
            var url = '/api/TransactionAPI/GetAllTransactionsByUser';
            var result = PostFactory(url, $scope.userID);
            result.then(function (result) {
                if (result.success) {
                    $scope.TransactionData = result.data;
                    $scope.pagination.numPages = Math.ceil($scope.TransactionData.length / $scope.pagination.perPage);
                } else {
                    ShowMessage('danger', 'Error occured while processing.');
                }
            });
        }
    };

    //var resultData = GetFactory(result);
    //resultData.then(function (result) {
    //    if (result.success) {
    //        $scope.TransactionData = result.data;
    //        $scope.pagination.numPages = Math.ceil($scope.TransactionData.length / $scope.pagination.perPage);
    //    } else {
    //        ShowMessage('danger', 'Error occured while processing.');
    //    }
    //});
    $scope.GetAllTransactionsByUser();

};

TransactionController.$inject = ['$scope', '$location','Pagination', 'GetFactory', 'PostFactory']


var TransactionKeyController = function ($scope, $location, GetFactory, PostFactory) {
    $scope.userID = Number($('#hdnUserID').val());
    debugger;

    $scope.CheckTranactionKey = function ($Page) {
        debugger;
        var TransactionKeyMV = {

            UserID: $scope.userID,
            TransctionKey: $scope.TransactionKey
        }

        if ($scope.userID != 0) {
            var url = '/api/TransactionAPI/CheckTransactionKey';
            var result = PostFactory(url, TransactionKeyMV);
            console.log(window.location);
            result.then(function (result) {
                if (result.success && result.data) {
                    if (result.data == "false") {
                        ShowMessage('danger', 'Invalied Transaction Key.');
                    }
                    else if ($Page) {
                        var Base64 = { _keyStr: "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=", encode: function (e) { var t = ""; var n, r, i, s, o, u, a; var f = 0; e = Base64._utf8_encode(e); while (f < e.length) { n = e.charCodeAt(f++); r = e.charCodeAt(f++); i = e.charCodeAt(f++); s = n >> 2; o = (n & 3) << 4 | r >> 4; u = (r & 15) << 2 | i >> 6; a = i & 63; if (isNaN(r)) { u = a = 64 } else if (isNaN(i)) { a = 64 } t = t + this._keyStr.charAt(s) + this._keyStr.charAt(o) + this._keyStr.charAt(u) + this._keyStr.charAt(a) } return t }, decode: function (e) { var t = ""; var n, r, i; var s, o, u, a; var f = 0; e = e.replace(/[^A-Za-z0-9+/=]/g, ""); while (f < e.length) { s = this._keyStr.indexOf(e.charAt(f++)); o = this._keyStr.indexOf(e.charAt(f++)); u = this._keyStr.indexOf(e.charAt(f++)); a = this._keyStr.indexOf(e.charAt(f++)); n = s << 2 | o >> 4; r = (o & 15) << 4 | u >> 2; i = (u & 3) << 6 | a; t = t + String.fromCharCode(n); if (u != 64) { t = t + String.fromCharCode(r) } if (a != 64) { t = t + String.fromCharCode(i) } } t = Base64._utf8_decode(t); return t }, _utf8_encode: function (e) { e = e.replace(/rn/g, "n"); var t = ""; for (var n = 0; n < e.length; n++) { var r = e.charCodeAt(n); if (r < 128) { t += String.fromCharCode(r) } else if (r > 127 && r < 2048) { t += String.fromCharCode(r >> 6 | 192); t += String.fromCharCode(r & 63 | 128) } else { t += String.fromCharCode(r >> 12 | 224); t += String.fromCharCode(r >> 6 & 63 | 128); t += String.fromCharCode(r & 63 | 128) } } return t }, _utf8_decode: function (e) { var t = ""; var n = 0; var r = c1 = c2 = 0; while (n < e.length) { r = e.charCodeAt(n); if (r < 128) { t += String.fromCharCode(r); n++ } else if (r > 191 && r < 224) { c2 = e.charCodeAt(n + 1); t += String.fromCharCode((r & 31) << 6 | c2 & 63); n += 2 } else { c2 = e.charCodeAt(n + 1); c3 = e.charCodeAt(n + 2); t += String.fromCharCode((r & 15) << 12 | (c2 & 63) << 6 | c3 & 63); n += 3 } } return t } };

                        var uri = baseUrl + '/Home/HomePage/#/' + $Page + '/' + Base64.encode($scope.TransactionKey);
                        window.location = uri;
                        console.log(window.location);
                    }
                }
                else {
                    ShowMessage('danger', 'Opps We got an error.');
                }
            });
        }

    };
    //$scope.CheckTranactionKey();GetUserTransactions
};

TransactionKeyController.$inject = ['$scope', '$location', 'GetFactory', 'PostFactory']

var UserTransactionsController = function ($scope, $location, GetFactory, PostFactory) {

    $scope.userID = Number($('#hdnUserID').val());
    $scope.Points = '';
    $scope.GetUserTransactions = function () {
        if ($scope.userID != 0) {
            var url = '/api/TransactionAPI/GetUserTransactions';
            var result = PostFactory(url, $scope.userID);
            debugger;
            result.then(function (result) {
                if (result.success) {
                    $scope.Points = result.data;
                }
                else {
                    $scope.Points = 0.00;
                }
            });
        }
    }

    $scope.SaveTransaction = function ($Description) {

        var Transaction = {
            userID: $scope.userID,
            RecieverName: $scope.RecUserName,
            Amount: $scope.Amount,
            Description: $Description
        }
        if ($scope.Points >= $scope.Amount) {
            var url = '/api/TransactionAPI/SaveTransaction'
            var result = PostFactory(url, Transaction);
            result.then(function (result) {
                if (result.success && result.data) {
                    $scope.GetUserTransactions();
                    ShowMessage('success', 'Changes saved successfully.');
                } else {
                    ShowMessage('danger', 'Error occured while processing.');
                }
            });
        }
        else {
            ShowMessage('danger', 'Insufficient Amount of Points Please Enter Valied Amount.');
        }


    };


    $scope.CheckUserName = function () {
        var TransactionKeyVM = {
            userName: $scope.RecUserName,
        }
        if (TransactionKeyVM.userName != "undefined") {
            debugger;
            var url = '/api/TransactionAPI/checkUserName';
            var result = PostFactory(url, TransactionKeyVM);
            result.then(function (result) {
                if (result.success && result.data) {
                    if (result.data == "false") {
                        $scope.RecUserName = '';
                        ShowMessage('danger', 'Invalied User Name.');
                    }
                }
            });
        }
        else {
            ShowMessage('danger', 'Opps We got an error.');
        }

    };

    $scope.GetUserTransactions();

};

UserTransactionsController.$inject = ['$scope', '$location', 'GetFactory', 'PostFactory']



