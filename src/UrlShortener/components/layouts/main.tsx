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
      <Container pt={20} maxW="container.md">
        {children}
      </Container>
      {/* <Footer /> */}
    </Box>
  )
}

export default Main
