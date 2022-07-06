import {
  Alert,
  AlertIcon,
  Box,
  Container,
  Heading,
  HStack,
  Stack,
  useBreakpointValue,
  useColorModeValue,
  Text,
  PinInputField,
  PinInput,
  Progress,
  Center
} from '@chakra-ui/react'
import { Logo } from '@components/logo'
import { ApiRoutes } from '@models/apiRoutes'
import { GetServerSideProps } from 'next'
import { useRouter } from 'next/router'
import { useState } from 'react'

const Email = () => {
  const router = useRouter()
  const email = router.query.email as string

  const [alertText, setAlertText] = useState('')
  const [alertVisible, setAlertVisibility] = useState(false)

  const [inProgress, setInProgress] = useState(false)

  return (
    <Container
      maxW="lg"
      py={{ base: '0', md: '18' }}
      px={{ base: '0', sm: '8' }}
    >
      <Stack spacing="4">
        <Stack spacing="6">
          <Logo />
          <Stack spacing={{ base: '2', md: '3' }} textAlign="center">
            <Heading size={useBreakpointValue({ base: 'md', md: 'lg' })}>
              Verify your email address
            </Heading>
            <Text>
              Enter the six digit code we sent to your email address in order to
              confirm your account
            </Text>
          </Stack>
        </Stack>
        <Box
          py={{ base: '0', sm: '8' }}
          px={{ base: '4', sm: '10' }}
          bg={useBreakpointValue({ base: 'transparent', sm: 'bg-surface' })}
          boxShadow={{ base: 'none', sm: useColorModeValue('md', 'md-dark') }}
          borderRadius={{ base: 'none', sm: 'xl' }}
        >
          <Stack spacing="6">
            <Progress size="xs" isIndeterminate={inProgress} />
            {alertVisible && (
              <Alert status="error" variant="subtle">
                <AlertIcon />
                {alertText}
              </Alert>
            )}
            <Center>
              <HStack>
                <PinInput
                  onComplete={async token => {
                    setInProgress(true)
                    await fetchConfirmEmail(email, token)
                    setInProgress(false)
                  }}
                >
                  <PinInputField />
                  <PinInputField />
                  <PinInputField />
                  <PinInputField />
                  <PinInputField />
                  <PinInputField />
                </PinInput>
              </HStack>
            </Center>
          </Stack>
        </Box>
      </Stack>
    </Container>
  )

  async function fetchConfirmEmail(email: string, token: string) {
    process.env['NODE_TLS_REJECT_UNAUTHORIZED'] = '0'
    const encodedEmail = encodeURIComponent(email)

    const result = await fetch(
      `${ApiRoutes.Account.Email.Confirm}?email=${encodedEmail}&token=${token}`
    )

    if (result.ok) {
      router.push('/account/login')
      return
    } else if (result.status === 400) setAlertText('Not found')
    else if (result.status === 409) setAlertText('Email is already confirmed')
    else if (result.status === 404) setAlertText('404')
    else setAlertText('Something went wrong...')

    setAlertVisibility(true)
  }
}

export const getServerSideProps: GetServerSideProps = async ({ query }) => {
  process.env['NODE_TLS_REJECT_UNAUTHORIZED'] = '0'

  if (!query.email)
    return {
      notFound: true
    }

  const email = query.email as string
  const encodedEmail = encodeURIComponent(email)

  const result = await fetch(
    `${ApiRoutes.Account.Email.IsConfirmed}?email=${encodedEmail}`
  )

  if (result.ok)
    return {
      props: {}
    }

  return {
    notFound: true
  }
}

export default Email
