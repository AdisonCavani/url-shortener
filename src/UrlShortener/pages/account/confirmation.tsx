import { useRouter } from 'next/router'
import ErrorPage from 'next/error'

const ConfirmationPage = () => {
  const router = useRouter()
  if (!router.query.email) {
    return <ErrorPage statusCode={404} />
  } else {
    return <div>{router.query.email}</div>
  }
}

export default ConfirmationPage
