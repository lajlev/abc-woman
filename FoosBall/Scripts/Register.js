jQuery(window).load(function() {
    $('#Email').focus();

    // Form Validation
    var $inputFields = $('input');
    var emailRegExp = /[@]/i;

    $('form[name="register-player"]').on('submit', function (e) {
        var email = $('#Email').val(),
            missingFields = false;

        $.each($('input[type="text"], input[type="email"]'), function () {
            var $this = $(this);
            $this.val($this.val().trim());
        });

        // Validate that all fields are filled out
        $.each($inputFields, function () {
            var $this = $(this);
            if (!!$this.val() === false) {
                missingFields = true;
                return;
            }
        });

        if (missingFields) {
            displayErrorMessage("All fields are required to register.","All");
        } else {
            clearErrorMessage("All");
        }

        // Validate emails field
        if (emailRegExp.test(email) === true) {
            log("email contains an '@'");
            displayErrorMessage("You must submit a valid trustpilot email.", "Email");
        } else {
            emailExists(email);
        }

        // Validate Name field
        nameExists($('#Name').val());

        // Validate password fields
        if ($('#Password').val() !== $('#Repeat-Password').val()) {
            displayErrorMessage("Your passwords do not match.", "Password");
        } else {
            clearErrorMessage("Password");
        }
        
        // Check if errors occured 
        if (errorState()) {
            e.preventDefault();
        }
    });

    $('#Email').on('change', function() {
        var $nickName = $('#NickName');
        if (!!$nickName.val() === false) {
            $nickName.val($(this).val().substr(0,5));
        }
    });
});

// Synch call to server to check if email is alredy registered
function emailExists(email) {
    $.ajax({
        url: '/Account/PlayerEmailExists',
        data: { email: email },
        dataType: 'json',
        async: false,
        success: function (response) {
            if (response.Exists) {
                displayErrorMessage("A player with this email already exists ("+response.Name+")", "Email");
            } else {
                clearErrorMessage("Email");
            }
        }
    });
}

// Synch call to server to check if name is alredy registered
function nameExists(name) {
    $.ajax({
        url: '/Account/PlayerNameExists',
        data: { name: name },
        dataType: 'json',
        async: false,
        success: function (response) {
            if (response.Exists) {
                displayErrorMessage("A player with this name already exists (" + response.Email + ")", "Name");
            } else {
                clearErrorMessage("Name");
            }
        }
    });
}