var InviteFriendsController = function ($scope, $location, GetFactory, PostFactory) {

    $scope.sharingUrl = $('#hdnSharingUrl').val();
    $scope.friendsemails = '';
    $scope.showLoading = false;
    $scope.userID = Number($('#hdnUserID').val());

    $scope.copyURL = function () {
        /* Get the text field */
        var copyText = document.getElementById("txtSharingUrl");

        /* Select the text field */
        copyText.select();

        /* Copy the text inside the text field */
        document.execCommand("Copy");

        /* Alert the copied text */
        ShowMessage('success', 'Sharing URL copied to your clipboard. Now you can paste it and share it among your contacts.');
    }

    $scope.sendInvitations = function () {
        $scope.showLoading = true;
        var data = {
            UserID: $scope.userID,
            EmailAddressList: $scope.friendsemails
        }
        var url = '/api/BackEndAPI/SendInvitations'
        var result = PostFactory(url, data);
        result.then(function (result) {
            if (result.success && result.data) {
                ShowMessage('success', 'Invitations sent successfully.');
                //$location.path('/AdminPendingDeposits');
            } else {
                ShowMessage('danger', 'Error occured while processing.');
            }
            $scope.showLoading = false;
        });
    }

    
}

InviteFriendsController.$inject = ['$scope', '$location', 'GetFactory', 'PostFactory']