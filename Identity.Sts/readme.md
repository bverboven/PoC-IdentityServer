Identity Security Token Service
****

x. Username
Email is used as Username and should be unique.

x. Seeding
Added UserRole "Administrator" with claims (can_read, can_create, can_update, can_delete).
Created AdminUser with role Administrator

x. AccountController
- Added 2 factor authentication option (Login2fa)
- Redirects to LoginWith2faController when 2fa is enabled

x. LoginWith2faController
- Added with view (copied code from Identity razor pages)
- Also added LoginWithRecoveryCodeController for recovery

x. ExternalController
- Automatically creates a new user with the email as username
- Adding given_name and family_name as claims

x. Sign in
SignInManager.IsSignedIn returns false since the Identity was created with AuthenticationType = "AuthenticationTypes.Federation",
but was compared with *IdentityConstants.ApplicationScheme*. Now both AuthenicationTypes are allowed to be signed in.

x. Claims
- Custom ApplicationUserClaimsFactory
