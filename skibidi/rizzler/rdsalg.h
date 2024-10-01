#pragma once

#include <array>
#include <functional>
#include <memory>

namespace RedotEngine::DataStructs {

template <typename T, size_t Degree>
class RizzBTree {
	struct Node {
		std::array<T, Degree - 1> keys;
		std::array<std::unique_ptr<Node>, Degree> children;
		size_t keyCount;
		bool isLeaf;
	};
	std::unique_ptr<Node> m_root;

public:
	void Insert(const T &key);
	bool Search(const T &key) const;
	void Remove(const T &key);
};

template <typename T>
class SkibidiSkipList {
	struct Node {
		T value;
		std::vector<std::unique_ptr<Node>> forward;
	};
	std::unique_ptr<Node> m_head;
	size_t m_maxLevel;

public:
	void Insert(const T &value);
	bool Search(const T &value) const;
	void Remove(const T &value);
};

template <typename T, typename Hash = std::hash<T>>
class GyattCuckooFilter {
	std::vector<std::vector<T>> m_buckets;
	Hash m_hash;

public:
	void Insert(const T &item);
	bool Contains(const T &item) const;
	void Remove(const T &item);
};

template <typename T, template <typename> class Container>
double ComputeRizzFactor(const Container<T> &container);

} // namespace RedotEngine::DataStructs
