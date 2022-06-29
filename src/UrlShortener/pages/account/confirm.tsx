import axios from 'axios'
import { GetServerSideProps } from 'next'

export interface ConfirmProps {
  success: boolean
  errors?: string
  response?: object
}

export default function ConfirmPage(props: ConfirmProps) {
  if (!props.success) {
    return <div>{props.errors}</div>
  } else {
    console.log(props.response)
  }
}

export const getServerSideProps: GetServerSideProps = async context => {
  let email = encodeURIComponent(context!.query!.email!.toString())
  let token = encodeURIComponent(context!.query!.token!.toString())

  await fetch(
    `https://localhost:7081/api/v1/account/confirmEmail?email=${email}&token=${token}`
  )
    .then(res => {
      if (res.ok)
        return {
          props: {
            success: true,
            response: res.json()
          }
        }
      return {
        props: {
          success: false,
          errors: 'Something went wrong...'
        }
      }
    })
    .catch(error => {
      return {
        props: {
          success: false,
          errors: error
        }
      }
    })
  return {
    props: {
      success: false,
      errors: 'xd'
    }
  }
}
