var LandingPageController = function ($scope, $location, PostFactory) {
    $scope.models = {
        helloAngular: 'I work!'
    };

    makeMenuActive('Profile');

    $scope.SignOut = function () {
        debugger;
        var url = '/User/SignOut';
        var data = {};
        var result = PostFactory(url, data);
        result.then(function (result) {
            if (result.success) {
                window.location = baseUrl + '/User/Login';
            } else {
                ShowMessage('danger', 'Error occured while processing.');
            }
        });
    }
    $scope.userID = Number($('#hdnUserID').val());
    $scope.GetUserTransactions = function () {
        if ($scope.userID != 0) {
            var url = '/api/TransactionAPI/GetUserTransactions';
            //var result = PostFactory(url, $scope.userID);
            //debugger;
            //result.then(function (result) {
            //    if (result.success) {
            //        $scope.Points = result.data;
            //    }
            //    else {
            //        $scope.Points = 0.00;
            //    }
            //});
        }
    }

    $scope.GetUserTransactions();

}

LandingPageController.$inject = ['$scope', '$location', 'PostFactory'];