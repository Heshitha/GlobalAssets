var BuyPackageController = function ($scope, $location, GetFactory, PostFactory) {

    $scope.userID = Number($('#hdnUserID').val());
    $scope.packid = Number($('#hdnPackID').val());
    $scope.amount = Number($('#hdnAmount').val());
    $scope.minAmount = Number($('#hdnMinAmount').val());
    $scope.maxAmount = Number($('#hdnMaxAmount').val());
    $scope.upid = Number($('#hdnUPID').val());
    $scope.paymentMethod = '';
    $scope.transactionid = '';
    $scope.transactionemail = '';
    $scope.nettransactionid = '';
    $scope.nettransactionemail = '';
    $scope.skrtransactionid = '';
    $scope.skrtransactionemail = '';
    $scope.declineReason = $('#hdnReason').val();
    $scope.showLoading = false;

    $scope.buypackage = function () {
        debugger;
        if ($scope.minAmount <= $scope.amount && $scope.amount <= $scope.maxAmount) {

            $scope.showLoading = true;

            if ($scope.paymentMethod == 'Neteller') {
                $scope.transactionid = $('#txtNetTransactionID').val();
                $scope.transactionemail = $('#txtNetellerAccountEmail').val();
            } else if ($scope.paymentMethod == 'Skrill') {
                $scope.transactionid = $('#txtSkrTransactionID').val();
                $scope.transactionemail = $('#txtSkrillAccountEmail').val();
            }

            var data = {
                UserID: $scope.userID,
                PackID: $scope.packid,
                Amount: $scope.amount,
                PaymentType: $scope.paymentMethod,
                TransactionID: $scope.transactionid,
                TransactionMail: $scope.transactionemail
            };
            var url = '/api/BackEndAPI/BuyPackage'
            var result = PostFactory(url, data);
            result.then(function (result) {
                if (result.success && result.data) {
                    ShowMessage('success', 'Package purchased successfully. Our agents will approve your purchase soon.');
                    //$location.path('/DepositsHistory');
                    window.location = baseUrl + '/BackEnd/Index/#/DepositsHistory';
                } else {
                    ShowMessage('danger', 'Error occured while processing.');
                }
                $scope.showLoading = false;
            });
        }
        else {
            ShowMessage('danger', 'Amount should be within the package range. Please select correct package for amount you want to deposit');
        }
    }

    $scope.ApprovePackage = function () {
        $scope.showLoading = true;
        var data = {
            UserID: $scope.userID,
            UPID: $scope.upid
        }
        var url = '/api/BackEndAPI/ApprovePackage'
        var result = PostFactory(url, data);
        result.then(function (result) {
            if (result.success && result.data) {
                ShowMessage('success', 'Package activated successfully.');
                //$location.path('/AdminPendingDeposits');
                window.location = baseUrl + '/BackEnd/Index/#/AdminPendingDeposits';
            } else {
                ShowMessage('danger', 'Error occured while processing.');
            }
            $scope.showLoading = false;
        });
    }

    $scope.DeclinePackage = function () {
        $scope.showLoading = true;
        var data = {
            UserID: $scope.userID,
            UPID: $scope.upid,
            ReasonForDecline: $('#txtReasonForDecline').val()
        }
        var url = '/api/BackEndAPI/DeclinePackage'
        var result = PostFactory(url, data);
        result.then(function (result) {
            if (result.success && result.data) {
                ShowMessage('success', 'Package declined successfully.');
                //$location.path('/AdminPendingDeposits');
                window.location = baseUrl + '/BackEnd/Index/#/AdminPendingDeposits';
            } else {
                ShowMessage('danger', 'Error occured while processing.');
            }
            $scope.showLoading = false;
        });
    }
}

BuyPackageController.$inject = ['$scope', '$location', 'GetFactory', 'PostFactory']