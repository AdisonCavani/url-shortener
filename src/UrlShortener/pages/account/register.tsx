import {
  Alert,
  AlertIcon,
  Box,
  Button,
  Container,
  Divider,
  FormControl,
  FormErrorMessage,
  FormLabel,
  Grid,
  GridItem,
  Heading,
  HStack,
  Input,
  Stack,
  Text,
  useBreakpointValue,
  useColorModeValue
} from '@chakra-ui/react'
import { Field, Form, Formik } from 'formik'
import { Logo } from '@components/logo'
import { OAuthButtonGroup } from '@components/oauthButtonGroup'
import * as Yup from 'yup'
import Link from 'next/link'
import axios, { AxiosError } from 'axios'
import Router from 'next/router'
import { useState } from 'react'
import { ApiRoutes } from '@models/apiRoutes'

const RegisterPage = () => {
  const SigninSchema = Yup.object().shape({
    firstName: Yup.string()
      .min(2, 'Too Short!')
      .max(255, 'Too Long!')
      .required('This field is required'),
    lastName: Yup.string()
      .min(2, 'Too Short!')
      .max(255, 'Too Long!')
      .required('This field is required'),
    email: Yup.string()
      .email('Invalid email')
      .required('This field is required'),
    password: Yup.string()
      .min(8, 'Too Short!')
      .max(50, 'Too Long!')
      .required('This field is required')
  })

  const [alertText, setAlertText] = useState('Something went wrong...')
  const [alertVisible, setAlertVisibility] = useState(false)

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
              Create an account
            </Heading>
            <HStack spacing="1" justify="center">
              <Text color="muted">Already have an account?</Text>
              <Link href="./login">
                <Button variant="link" colorScheme="blue">
                  Log in
                </Button>
              </Link>
            </HStack>
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
            <Formik
              validationSchema={SigninSchema}
              initialValues={{
                firstName: '',
                lastName: '',
                email: '',
                password: ''
              }}
              onSubmit={async (values, actions) => {
                await fetchRegister(values)
                actions.setSubmitting(false)
              }}
            >
              {props => (
                <Form>
                  <Stack spacing="5">
                    {alertVisible && (
                      <Alert status="error" variant="subtle">
                        <AlertIcon />
                        {alertText}
                      </Alert>
                    )}
                    <Grid templateColumns="repeat(2, 1fr)" gap={4}>
                      <GridItem>
                        <Field name="firstName">
                          {({ field, form }) => (
                            <FormControl
                              isRequired={true}
                              isInvalid={
                                form.errors.firstName && form.touched.firstName
                              }
                            >
                              <FormLabel htmlFor="firstName">
                                First name
                              </FormLabel>
                              <Input id="firstName" {...field} />
                              <FormErrorMessage>
                                {form.errors.firstName}
                              </FormErrorMessage>
                            </FormControl>
                          )}
                        </Field>
                      </GridItem>
                      <GridItem>
                        <Field name="lastName">
                          {({ field, form }) => (
                            <FormControl
                              isRequired={true}
                              isInvalid={
                                form.errors.lastName && form.touched.lastName
                              }
                            >
                              <FormLabel htmlFor="lastName">
                                Last Name
                              </FormLabel>
                              <Input id="lastName" {...field} />
                              <FormErrorMessage>
                                {form.errors.lastName}
                              </FormErrorMessage>
                            </FormControl>
                          )}
                        </Field>
                      </GridItem>
                    </Grid>
                    <Field name="email">
                      {({ field, form }) => (
                        <FormControl
                          isRequired={true}
                          isInvalid={form.errors.email && form.touched.email}
                        >
                          <FormLabel htmlFor="email">Email</FormLabel>
                          <Input id="email" {...field} />
                          <FormErrorMessage>
                            {form.errors.email}
                          </FormErrorMessage>
                        </FormControl>
                      )}
                    </Field>
                    <Field name="password">
                      {({ field, form }) => (
                        <FormControl
                          isRequired={true}
                          isInvalid={
                            form.errors.password && form.touched.password
                          }
                        >
                          <FormLabel htmlFor="password">Password</FormLabel>
                          <Input
                            id="password"
                            name="password"
                            type="password"
                            autoComplete="new-password"
                            {...field}
                          />
                          <FormErrorMessage>
                            {form.errors.password}
                          </FormErrorMessage>
                        </FormControl>
                      )}
                    </Field>
                  </Stack>
                  <Stack spacing="6" mt={6}>
                    <Button
                      type="submit"
                      colorScheme="blue"
                      isLoading={props.isSubmitting}
                    >
                      Create Account
                    </Button>
                    <HStack>
                      <Divider />
                      <Text fontSize="sm" whiteSpace="nowrap" color="muted">
                        or sign up with
                      </Text>
                      <Divider />
                    </HStack>
                    <OAuthButtonGroup />
                  </Stack>
                </Form>
              )}
            </Formik>
          </Stack>
        </Box>
      </Stack>
    </Container>
  )

  async function fetchRegister(values) {
    process.env['NODE_TLS_REJECT_UNAUTHORIZED'] = '0'

    await axios
      .post(ApiRoutes.Account.Register, values)
      .then(res => {
        console.log(res)
        Router.push(`/account/confirm/${encodeURIComponent(values.email)}`)
      })
      .catch(error => {
        if (error!.response!.status !== 400) {
          console.log(error!.response!.data!)

          let message = error.response.data.errors

          console.log(message)

          setAlertText('')
        } else setAlertText('Something went wrong...')
        setAlertVisibility(true)
      })
  }
}

export default RegisterPage
