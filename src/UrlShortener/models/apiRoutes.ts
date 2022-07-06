export class ApiRoutes {
  static readonly root = 'https://localhost:7081'
  static readonly version = '1'
  static readonly baseUrl = `${this.root}/v${this.version}`

  static Account = class {
    private static readonly endpoint = `${ApiRoutes.baseUrl}/account`

    public static readonly Login = `${this.endpoint}/login`
    public static readonly Register = `${this.endpoint}/register`

    static Email = class {
      private static readonly endpoint = `${ApiRoutes.Account.endpoint}/email`

      public static readonly Confirm = `${this.endpoint}/confirm`
      public static readonly IsConfirmed = `${this.endpoint}/isConfirmed`
      public static readonly ResendVerification = `${this.endpoint}/resendVerification`
    }

    static Password = class {
      private static readonly endpoint = `${ApiRoutes.Account.endpoint}/password`

      public static readonly VerifyToken = `${this.endpoint}/verifyToken`
      public static readonly Reset = `${this.endpoint}/reset`
      public static readonly SendRecovery = `${this.endpoint}/recovery`

      public static readonly Change = `${this.endpoint}/change`
    }
  }
}
