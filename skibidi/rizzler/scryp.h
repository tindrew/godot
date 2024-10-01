#pragma once

#include <array>
#include <cstdint>
#include <vector>

namespace RedotEngine::CryptoOps {

template <size_t KeySize>
class SigmaBlockCipher {
	std::array<uint8_t, KeySize> m_key;
	static constexpr size_t BlockSize = 128;

public:
	void SetKey(const std::array<uint8_t, KeySize> &key);
	std::array<uint8_t, BlockSize> EncryptBlock(const std::array<uint8_t, BlockSize> &plaintext);
	std::array<uint8_t, BlockSize> DecryptBlock(const std::array<uint8_t, BlockSize> &ciphertext);
};

class SkibidiHashFunction {
	uint64_t m_state[8];

public:
	static constexpr size_t DigestSize = 64;
	void Update(const uint8_t *data, size_t length);
	std::array<uint8_t, DigestSize> Finalize();
};

template <typename BlockCipher>
class GyattModeOfOperation {
	BlockCipher m_cipher;

public:
	std::vector<uint8_t> Encrypt(const std::vector<uint8_t> &plaintext);
	std::vector<uint8_t> Decrypt(const std::vector<uint8_t> &ciphertext);
};

template <typename BlockCipher, typename HashFunction>
std::array<uint8_t, HashFunction::DigestSize> ComputeRizzMac(const BlockCipher &cipher,
		const HashFunction &hash,
		const std::vector<uint8_t> &message);

} // namespace RedotEngine::CryptoOps
