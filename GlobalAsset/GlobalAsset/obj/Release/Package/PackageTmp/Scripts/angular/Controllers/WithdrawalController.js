var WithdrawalController = function ($scope, $location, GetFactory, PostFactory) {

    $scope.userID = Number($('#hdnUserID').val());
    $scope.paymentMethod = '';
    $scope.amount = '';
    $scope.paymentEmail = '';
    $scope.totalEarnings = 0;
    $scope.successfullWithdraws = 0;
    $scope.pendingWithdraws = 0;
    $scope.availbleBalance = 0;
    $scope.recentWithdrawals = Array();
    $scope.showLoading = false;

    $scope.loadWithdrawals = function () {
        $scope.showLoading = true;
        var url = '/api/BackEndAPI/LoadWithdraw'
        var result = PostFactory(url, $scope.userID);
        result.then(function (result) {
            if (result.success && result.data != null) {
                $scope.totalEarnings = result.data.TotalEarnings;
                $scope.successfullWithdraws = result.data.SuccessfullWithdraws;
                $scope.pendingWithdraws = result.data.PendingWithdraws;
                $scope.availbleBalance = result.data.AvailbleBalance;
                $scope.recentWithdrawals = result.data.RecentWithdrawals;
            } else {
                //ShowMessage('danger', 'Error occured while processing.');
            }
            $scope.showLoading = false;
        });
    }

    $scope.loadWithdrawals();

    $scope.getPaymentEmail = function () {
        var data = {
            UserID: $scope.userID,
            PaymentType: $scope.paymentMethod
        }
        $scope.showLoading = true;
        var url = '/api/BackEndAPI/GetWithdrawEmail'
        var result = PostFactory(url, data);
        result.then(function (result) {
            if (result.success && result.data != null && result.data != "null") {
                debugger;
                $scope.paymentEmail = result.data.replace(/\"/g, '');
                //ShowMessage('success', 'Package activated successfully.');
            } else {
                $scope.paymentEmail = '';
            }
            $scope.showLoading = false;
        });
    }

    $scope.saveWithdrawal = function () {
        if ($scope.amount > $scope.availbleBalance) {
            ShowMessage('warning', 'You can\'t request this amount. Amount should less than or equal to available balance. Please try again.');
        }
        else if ($scope.amount < 10) {
            ShowMessage('warning', 'Amount must be greater than $10.00. Please try again.');
        }
        else {
            debugger;
            $scope.showLoading = true;
            var data = {
                UserID: $scope.userID,
                PaymentMethod: $scope.paymentMethod,
                PaymentEmail: $scope.paymentEmail,
                Amount: $scope.amount
            }
            var url = '/api/BackEndAPI/SaveWithdrawal'
            var result = PostFactory(url, data);
            result.then(function (result) {
                if (result.success && result.data) {
                    ShowMessage('success', 'Withdrawal requested successfully. Soon our agents will process your request.');
                    $scope.loadWithdrawals();
                } else {
                    ShowMessage('danger', 'Error occured while processing. Please try again.');
                }
                $scope.showLoading = false;
            });
        }
    }
}

WithdrawalController.$inject = ['$scope', '$location', 'GetFactory', 'PostFactory']