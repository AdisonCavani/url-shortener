import {
  Alert,
  AlertIcon,
  Box,
  Button,
  Checkbox,
  Container,
  Divider,
  FormControl,
  FormErrorMessage,
  FormLabel,
  Heading,
  HStack,
  IconButton,
  Input,
  InputGroup,
  InputRightElement,
  Stack,
  Text,
  useBreakpointValue,
  useColorModeValue,
  useDisclosure,
  useMergeRefs
} from '@chakra-ui/react'
import { Field, Form, Formik } from 'formik'
import { Logo } from '@components/logo'
import { OAuthButtonGroup } from '@components/oauthButtonGroup'
import * as Yup from 'yup'
import { HiEye, HiEyeOff } from 'react-icons/hi'
import Link from 'next/link'
import React, { useState } from 'react'
import axios, { AxiosError } from 'axios'
import Router from 'next/router'

const LoginPage = () => {
  const SigninSchema = Yup.object().shape({
    email: Yup.string()
      .email('Invalid email')
      .required('This field is required'),
    password: Yup.string()
      .min(8, 'Too Short!')
      .max(50, 'Too Long!')
      .required('This field is required')
  })

  const { isOpen, onToggle } = useDisclosure()
  const inputRef = React.useRef<HTMLInputElement>(null)

  const mergeRef = useMergeRefs(inputRef, null)
  const onClickReveal = () => {
    onToggle()
    if (inputRef.current) {
      inputRef.current.focus({ preventScroll: true })
    }
  }

  const [alertText, setAlertText] = useState('')
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
              Log in to your account
            </Heading>
            <HStack spacing="1" justify="center">
              <Text color="muted">Don&apos;t have an account?</Text>
              <Link href="./register">
                <Button variant="link" colorScheme="blue">
                  Sign up
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
              initialValues={{ email: '', password: '' }}
              onSubmit={async (values, actions) => {
                await axios
                  .post('https://localhost:7081/api/v1/account/login', values)
                  .then(res => {
                    console.log(res)
                    setAlertVisibility(false)
                    Router.push('/')
                  })
                  .catch((error: AxiosError) => {
                    if (error!.response!.status == 409)
                      setAlertText('Confirm your email first!')
                    else if (error!.response!.status == 403)
                      setAlertText('Too many attemps... Try again later!')
                    else if (error!.response!.status == 400)
                      setAlertText('Invalid credentials!')
                    else setAlertText('Something went wrong...')
                    setAlertVisibility(true)
                  })

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
                    <Field name="email">
                      {({ field, form }) => (
                        <FormControl
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
                          isInvalid={
                            form.errors.password && form.touched.password
                          }
                        >
                          <FormLabel htmlFor="password">Password</FormLabel>
                          <InputGroup>
                            <InputRightElement>
                              <IconButton
                                variant="link"
                                aria-label={
                                  isOpen ? 'Mask password' : 'Reveal password'
                                }
                                icon={isOpen ? <HiEyeOff /> : <HiEye />}
                                onClick={onClickReveal}
                              />
                            </InputRightElement>
                            <Input
                              id="password"
                              ref={mergeRef}
                              name="password"
                              type={isOpen ? 'text' : 'password'}
                              autoComplete="current-password"
                              {...field}
                            />
                          </InputGroup>
                          <FormErrorMessage>
                            {form.errors.password}
                          </FormErrorMessage>
                        </FormControl>
                      )}
                    </Field>
                  </Stack>
                  <HStack justify="space-between" pt={6} pb={6}>
                    <Field
                      as={Checkbox}
                      id="rememberMe"
                      name="rememberMe"
                      defaultChecked
                    >
                      Remember me
                    </Field>
                    <Button variant="link" colorScheme="blue" size="sm">
                      Forgot password?
                    </Button>
                  </HStack>
                  <Stack spacing="6">
                    <Button
                      type="submit"
                      colorScheme="blue"
                      isLoading={props.isSubmitting}
                    >
                      Sign in
                    </Button>
                    <HStack>
                      <Divider />
                      <Text fontSize="sm" whiteSpace="nowrap" color="muted">
                        or continue with
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
}

export default LoginPage
