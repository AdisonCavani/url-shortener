import {
  Box,
  Button,
  ButtonGroup,
  Container,
  Flex,
  HStack,
  IconButton,
  Link,
  Menu,
  MenuButton,
  MenuItem,
  MenuList,
  useBreakpointValue,
  useColorModeValue
} from '@chakra-ui/react'
import { HamburgerIcon } from '@chakra-ui/icons'
import { Logo2 } from '../logo2'
import NextLink from 'next/link'

const Navbar = () => {
  return (
    <Box
      position="fixed"
      w="100%"
      as="nav"
      bg="bg-surface"
      zIndex={2}
      css={{ backdropFilter: 'blur(10px)' }}
      boxShadow={useColorModeValue('sm', 'sm-dark')}
    >
      <Container py={{ base: '4', lg: '5' }}>
        <HStack spacing="10" justify="space-between">
          <Logo2 />
          <Flex
            justify="space-between"
            flex="1"
            display={{ base: 'none', md: 'flex' }}
          >
            {/* <ButtonGroup variant="link" spacing="8">
                  {['Product', 'Pricing', 'Resources', 'Support'].map(item => (
                    <Button key={item}>{item}</Button>
                  ))}
                </ButtonGroup> */}
            <HStack spacing="3">
              <NextLink href="/account/login" passHref>
                <Button variant="ghost">Sign in</Button>
              </NextLink>
              <NextLink href="/account/register" passHref>
                <Button colorScheme="blue">Sign up</Button>
              </NextLink>
            </HStack>
          </Flex>
          <Box flex={1} textAlign="right">
            <Box ml={2} display={{ base: 'inline-block', md: 'none' }}>
              <Menu isLazy id="navbar-menu">
                <MenuButton
                  as={IconButton}
                  icon={<HamburgerIcon />}
                  variant="outline"
                  aria-label="Options"
                />
                <MenuList>
                  <NextLink href="/account/login" passHref>
                    <MenuItem>Login</MenuItem>
                  </NextLink>
                  <NextLink href="/account/register" passHref>
                    <MenuItem>Register</MenuItem>
                  </NextLink>
                  <MenuItem
                    isExternal
                    as={Link}
                    href="https://github.com/adisoncavani/url-shortener"
                  >
                    View Source
                  </MenuItem>
                </MenuList>
              </Menu>
            </Box>
          </Box>
        </HStack>
      </Container>
    </Box>
  )
}

export default Navbar
