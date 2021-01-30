var ViewWithdrawalController = function ($scope, $location, GetFactory, PostFactory) {

    $scope.admID = Number($('#hdnadmid').val());
    $scope.withid = Number($('#hdnwithid').val());
    $scope.isadmin = false;
    $scope.name = '';
    $scope.username = '';
    $scope.profileImage = '';
    $scope.userID = '';
    $scope.requesteddate = '';
    $scope.amount = 0;
    $scope.paymenttype = '';
    $scope.paymentemail = '';
    $scope.status = '';
    $scope.transactionid = '';
    $scope.sendersemail = '';
    $scope.processeddate = '';
    $scope.declinereason = '';
    $scope.amountwithoutsurcharge = 0;
    $scope.showLoading = false;

    $scope.loadWithdrawal = function () {
        $scope.showLoading = true;
        var url = '/api/BackEndAPI/GetWithdrawalDetails'
        var result = PostFactory(url, $scope.withid);
        result.then(function (result) {
            if (result.success && result.data != null) {
                $scope.name = result.data.Name;
                $scope.username = result.data.UserName;
                $scope.profileImage = result.data.ProfileImage;
                $scope.userID = result.data.UserID;
                $scope.requesteddate = result.data.RequestedDate;
                $scope.amount = result.data.Amount.toFixed(2);
                $scope.amountwithoutsurcharge = (Number(result.data.Amount) * 0.95).toFixed(2);
                $scope.paymenttype = result.data.PaymentType;
                $scope.paymentemail = result.data.PaymentEmail;
                $scope.status = result.data.Status;
                $scope.transactionid = result.data.TransactionId;
                $scope.sendersemail = result.data.SendersEmail;
                $scope.processeddate = result.data.ProcessedDate;
                $scope.declinereason = result.data.DeclineReason;
                $scope.isadmin = $scope.admID == 0 ? false : true;
            } else {
                //ShowMessage('danger', 'Error occured while processing.');
            }
            $scope.showLoading = false;
        });
    }

    $scope.loadWithdrawal();

    $scope.approveWithdraw = function () {
        debugger;
        if ($scope.transactionid != null && $scope.sendersemail != null) {
            $scope.showLoading = true;
            var data = {
                WithdrawID: $scope.withid,
                ApprovingUserID: $scope.admID,
                TransactionId: $scope.transactionid,
                SendersEmail: $scope.sendersemail
            };
            var url = '/api/BackEndAPI/ApproveWithdrawalDetails'
            var result = PostFactory(url, data);
            result.then(function (result) {
                if (result.success && result.data) {
                    ShowMessage('success', 'Withdrawal approved successfully.');
                    window.location = baseUrl + '/BackEnd/Index/#/PendingWithdrawals';
                    //$scope.loadWithdrawal();
                } else {
                    ShowMessage('danger', 'Error occured while processing. Please try again.');
                }
                $scope.showLoading = false;
            });

        } else {
            ShowMessage('warning', 'Please fill transaction id and sender\s email as these fields are required when approving the withdrawal. And try again.');
        }
    }

    $scope.declineWithdraw = function () {
        debugger;
        if ($scope.declinereason != null) {
            $scope.showLoading = true;
            var data = {
                WithdrawID: $scope.withid,
                ApprovingUserID: $scope.admID,
                DeclineReason: $scope.declinereason
            };
            var url = '/api/BackEndAPI/DeclineWithdrawal'
            var result = PostFactory(url, data);
            result.then(function (result) {
                if (result.success && result.data) {
                    ShowMessage('success', 'Withdrawal declined successfully.');
                    $scope.loadWithdrawal();
                } else {
                    ShowMessage('danger', 'Error occured while processing. Please try again.');
                }
                $scope.showLoading = false;
            });

        } else {
            ShowMessage('warning', 'Please fill reason for decline as the field is required when declining the withdrawal. And try again.');
        }
    }

}

ViewWithdrawalController.$inject = ['$scope', '$location', 'GetFactory', 'PostFactory']