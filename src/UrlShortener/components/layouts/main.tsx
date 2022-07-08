import { Box, Container } from '@chakra-ui/react'
import Head from 'next/head'
import Navbar from './navbar'

const Main = ({ children, router }) => {
  return (
    <Box as="main" pb={8}>
      <Head>
        <meta name="viewport" content="width=device-width, initial-scale=1" />
        <title>url-shortener</title>
      </Head>

      <Navbar />
      <Container pt={28} maxW="container.md">
        {children}
      </Container>
    </Box>
  )
}

export default Main
