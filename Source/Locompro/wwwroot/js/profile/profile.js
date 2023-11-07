/**
 * This module initializes and manages the UI behavior of the profile page.
 * Sets up the initial behavior when the document is ready.
 * @module profile-main
 */
import {ProfileModalManager} from './profile-modal-manager.js';
import {ProvinceHandler} from './province-handler.js';

/**
 * On ready it creates instances of `ProfileModalManager` and `ProvinceHandler`
 * to manage modals and province-canton selection in UpdateUserDataModal
 * It sets up an event listener for the change event on the provinces dropdown.
 */
$(document).ready(() => {
    const changePasswordModal = new ProfileModalManager(
        '#ChangePasswordModal',
        '#ChangePasswordErrorList',
        '#ChangePasswordModalErrors'
    );

   
    
    const updateUserDataModal = new ProfileModalManager(
        '#UpdateUserDataModal',
        '#UpdateUserModalErrorsList',
        '#UpdateUserModalErrors'
    );

    const provinceHandler = new ProvinceHandler(
        '#UserDataUpdate_Province',
        '#UserDataUpdate_Canton',
        '?handler=Cantons'
    );
    // Only instantiate DeclineModeratorModal if the modal for declining a moderator role exists
    if ($('#DeclineModerationRoleModal').length && $('#DeclineModerationErrorList').length && $('#DeclineModalErrors').length) {
        const declineModeratorModal = new ProfileModalManager(
            '#DeclineModerationRoleModal',
            '#DeclineModerationErrorList',
            '#DeclineModalErrors'
        );
        // Show modal and clear any existing errors when modal is closed
        declineModeratorModal.showModal().clearErrors();
    }

    // Show modals and clear any existing errors when modal is closed
    changePasswordModal.showModal().clearErrors();
    updateUserDataModal.showModal().clearErrors();
    
    // Set up an event listener for the change event on the provinces dropdown
    $(provinceHandler.provincesDropdownId).change(() => provinceHandler.handleProvinceChange());
});
