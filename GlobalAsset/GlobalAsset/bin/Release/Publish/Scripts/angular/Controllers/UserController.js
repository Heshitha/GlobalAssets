var UserController = function ($scope, $location, GetFactory, PostFactory) {
    
    $scope.userID = Number($('#hdnUserID').val());
    $scope.title = '';
    $scope.name = '';
    $scope.address = '';
    $scope.country = '';
    $scope.email = '';
    $scope.mobile = '';
    $scope.telephone = '';
    $scope.userName = '';
    $scope.password = '';
    $scope.referralName = '';
    $scope.netellerEmail = '';
    $scope.skrillEmail = '';
    $scope.bitCoinAddress = '';
    $scope.profileImage = '';
    $scope.currentPassword = '';
    $scope.newPassword = '';
    $scope.confirmPassword = '';
    $scope.showLoading = false;


    $scope.loadUserDetails = function () {
        if ($scope.userID != 0) {
            $scope.showLoading = true;
            var url = '/api/UserAPI/GetUserDetails';
            var result = PostFactory(url, $scope.userID);
            result.then(function (result) {
                if (result.success) {
                    debugger;
                    $scope.title = result.data.Title;
                    $scope.userName = result.data.UserName;
                    $scope.name = result.data.Name;
                    $scope.address = result.data.Address;
                    $scope.country = result.data.Country;
                    $scope.mobile = result.data.Mobile;
                    $scope.telephone = result.data.Telephone;
                    $scope.email = result.data.Email;
                    $scope.profileImage = result.data.ProfileImage;
                    $scope.netelleremail = result.data.NetellerEmail;
                    $scope.skrillaccountemail = result.data.SkrillEmail;
                    $scope.bitcoinwalletaddress = result.data.BitCoinAddress;
                } else {
                    ShowMessage('danger', 'Error occured while processing.');
                }
                $scope.showLoading = false;
            });
        }
    }

    $scope.loadUserDetails();

    $scope.uploadPreviewImage = function (element) {
        debugger;
        var uploader = $('#txtChooseFile')[0];
        if (uploader.files && uploader.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $('#imgPreview').attr('src', e.target.result);
                $('#imgPreview').show();
            };

            reader.readAsDataURL(uploader.files[0]);

        } else {
            $('#imgPreview').hide();
        }
    }

    $scope.uploadProfileImage = function () {
        var uploader = $('#txtChooseFile')[0];
        if (uploader.files && uploader.files[0]) {
            $scope.showLoading = true;
            var data = new FormData();
            data.append('userID', $scope.userID);
            data.append('file', uploader.files[0]);
            debugger;
            var url = 'User/SaveProfileImage';
            $.ajax({
                url: url,
                type: 'POST',
                data: data,
                cache: false,
                contentType: false,
                processData: false,
                success: function (data) {
                    ShowMessage('success', 'Image uploaded successfully.');
                    console.log(data);
                    location.reload();
                    $scope.showLoading = false;
                },
                error: function () {
                    ShowMessage('danger', 'Error occured while uploading. Please try again.');
                    $scope.showLoading = false;
                }

            });

        } else {
            ShowMessage('danger', 'Select image before uploading.');
        }
    }

    $scope.changePassword = function () {
        if ($scope.newPassword == $scope.confirmPassword) {
            $scope.showLoading = true;
            var user = {
                UserID: $scope.userID,
                Password: $scope.currentPassword,
                NewPassword: $scope.newPassword,
                ConfirmPassword: $scope.confirmPassword
            }
            var url = '/api/UserAPI/ChangePassword'
            var result = PostFactory(url, user);
            result.then(function (result) {
                if (result.success && result.data) {
                    ShowMessage('success', 'Password changed successfully.');
                    var signOutUrl = '/User/SignOut';
                    var result = PostFactory(signOutUrl);
                    result.then(function (result) {
                        if (result.success) {
                            window.location = baseUrl + '/User/Login';
                        }
                        $scope.showLoading = false;
                    });
                } else {
                    ShowMessage('danger', 'Error occured while processing.');
                }
                $scope.showLoading = false;
            });

        } else {
            ShowMessage('danger', 'New password and confirm password mismatched.');
        }
    }

    $scope.ModifyAccountDetails = function () {
        $scope.showLoading = true;
        var user = {
            UserID: $scope.userID,
            NetellerEmail: $scope.netelleremail,
            SkrillEmail: $scope.skrillaccountemail,
            BitCoinAddress: $scope.bitcoinwalletaddress
        }
        var url = '/api/UserAPI/ModifyAccountDetails'
        var result = PostFactory(url, user);
        result.then(function (result) {
            if (result.success && result.data) {
                ShowMessage('success', 'Account details updated successfully.');
            } else {
                ShowMessage('danger', 'Error occured while processing.');
            }
            $scope.showLoading = false;
        });
    }

    $scope.saveUserDetails = function () {
        $scope.showLoading = true;
        var user = {
            UserID: $scope.userID,
            Title: $scope.title,
            Address: $scope.address,
            Country: $scope.country,
            Mobile: $scope.mobile,
            Telephone: $scope.telephone,
        }
        var url = '/api/UserAPI/UpdateUserDetails'
        var result = PostFactory(url, user);
        result.then(function (result) {
            if (result.success && result.data) {
                ShowMessage('success', 'Changes saved successfully.');
            } else {
                ShowMessage('danger', 'Error occured while processing.');
            }
            $scope.showLoading = false;
        });
    }
}

UserController.$inject = ['$scope', '$location', 'GetFactory', 'PostFactory']