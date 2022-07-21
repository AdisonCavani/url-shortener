const root = 'https://localhost:7081'
const version = '1'
const baseUrl = `${root}/api/v${version}`

const accountEndpoint = `${baseUrl}/account`
const emailEndpoint = `${accountEndpoint}/email`
const passwordEndpoint = `${accountEndpoint}/password`

export class ApiRoutes {
  public static Account = class {
    public static readonly Login = `${accountEndpoint}/login`
    public static readonly Register = `${accountEndpoint}/register`

    public static Email = class {
      public static readonly Confirm = `${emailEndpoint}/confirm`
      public static readonly IsConfirmed = `${emailEndpoint}/isConfirmed`
      public static readonly ResendVerification = `${emailEndpoint}/resendVerification`
    }

    public static Password = class {
      public static readonly VerifyToken = `${passwordEndpoint}/verifyToken`
      public static readonly Reset = `${passwordEndpoint}/reset`
      public static readonly SendRecovery = `${passwordEndpoint}/recovery`

      public static readonly Change = `${passwordEndpoint}/change`
    }
  }
}
