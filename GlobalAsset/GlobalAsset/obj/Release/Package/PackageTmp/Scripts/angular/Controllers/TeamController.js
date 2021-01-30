var TeamViewerController = function ($scope, $location, GetFactory, PostFactory) {
    $scope.userID = Number($('#hdnUserID').val());
    $scope.startDate = '';
    $scope.endDate = '';
    $scope.Team = Array();
    $scope.TeamCount = 0;

    $scope.loadTeamDetails = function () {
        if ($scope.userID != 0) {
            var obj = {
                UserID: $scope.userID,
                startDate: $scope.startDate,
                endDate: $scope.endDate
            };
            var url = '/api/TeamAPI/GetTeamDetails';
            var result = PostFactory(url, obj);
            result.then(function (result) {
                if (result.success) {
                    $scope.Team = result.data;
                    $scope.TeamCount = result.data.length;
                    console.log(result.data);
                } else {
                    ShowMessage('danger', 'Error occured while processing.');
                }
            });
        }
    }
}

TeamViewerController.$inject = ['$scope', '$location', 'GetFactory', 'PostFactory']