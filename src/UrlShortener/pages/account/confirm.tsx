import { ErrorResponse } from '@models/errorResponse'
import { GetServerSideProps } from 'next'
import { ParsedUrlQuery } from 'querystring'

type Props = {
  errors?: string
}

interface Params extends ParsedUrlQuery {
  email: string
  token: string
}

const ConfirmPage = (props: Props) => {
  if (props.errors) {
    return <div>{props?.errors}</div>
  } else {
    return <div>Email confired successfuly</div>
  }
}

export const getServerSideProps: GetServerSideProps = async ({ query }) => {
  process.env.NODE_TLS_REJECT_UNAUTHORIZED = '0'

  if (!query?.email || !query?.token)
    return {
      notFound: true
    }

  const { email, token } = query as Params

  const encodedEmail = encodeURIComponent(email)
  const encodedToken = encodeURIComponent(token)

  const result = await fetch(
    `https://localhost:7081/api/v1/account/confirmEmail?email=${encodedEmail}&token=${encodedToken}`
  )

  if (result.ok)
    return {
      props: {}
    }

  if (result.status === 404) {
    return {
      notFound: true
    }
  }

  if (result.status === 409)
    return {
      props: {
        errors: 'Email is already confirmed'
      }
    }

  if (result.status === 400) {
    const errorsArr = (await result.json()) as ErrorResponse
    const errorMessage = errorsArr.errors?.join('\n')

    return {
      props: {
        errors: errorMessage
      }
    }
  }

  return {
    props: { errors: 'Something went wrong...' }
  }
}

export default ConfirmPage
