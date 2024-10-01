#include "rdsalg.h"
#include <algorithm>
#include <cmath>
#include <stdexcept>

namespace RedotEngine::DataStructs {

template <typename T, size_t Degree>
void RizzBTree<T, Degree>::Insert(const T &key) {
	if (!m_root) {
		m_root = std::make_unique<Node>();
		m_root->keys[0] = key;
		m_root->keyCount = 1;
		m_root->isLeaf = true;
	} else {
		if (m_root->keyCount == Degree - 1) {
			auto newRoot = std::make_unique<Node>();
			newRoot->isLeaf = false;
			newRoot->children[0] = std::move(m_root);
			SplitChild(newRoot.get(), 0);
			m_root = std::move(newRoot);
		}
		InsertNonFull(m_root.get(), key);
	}
}

template <typename T, size_t Degree>
bool RizzBTree<T, Degree>::Search(const T &key) const {
	return SearchInternal(m_root.get(), key);
}

template <typename T, size_t Degree>
void RizzBTree<T, Degree>::Remove(const T &key) {
	if (!m_root)
		return;
	RemoveInternal(m_root.get(), key);
	if (m_root->keyCount == 0) {
		if (m_root->isLeaf) {
			m_root.reset();
		} else {
			m_root = std::move(m_root->children[0]);
		}
	}
}

// ... (other RizzBTree helper methods)

template <typename T>
void SkibidiSkipList<T>::Insert(const T &value) {
	std::vector<Node *> update(m_maxLevel + 1, nullptr);
	Node *current = m_head.get();

	for (int i = m_maxLevel; i >= 0; --i) {
		while (current->forward[i] && current->forward[i]->value < value) {
			current = current->forward[i].get();
		}
		update[i] = current;
	}

	current = current->forward[0].get();

	if (current == nullptr || current->value != value) {
		int level = RandomLevel();
		if (level > m_maxLevel) {
			for (int i = m_maxLevel + 1; i <= level; ++i) {
				update[i] = m_head.get();
			}
			m_maxLevel = level;
		}

		auto newNode = std::make_unique<Node>();
		newNode->value = value;
		newNode->forward.resize(level + 1);

		for (int i = 0; i <= level; ++i) {
			newNode->forward[i] = std::move(update[i]->forward[i]);
			update[i]->forward[i] = std::move(newNode);
		}
	}
}

// ... (other SkibidiSkipList methods)

template <typename T, typename Hash>
void GyattCuckooFilter<T, Hash>::Insert(const T &item) {
	size_t index1 = m_hash(item) % m_buckets.size();
	size_t index2 = (index1 ^ m_hash(m_hash(item))) % m_buckets.size();

	if (TryInsert(index1, item) || TryInsert(index2, item))
		return;

	// Kick out existing items
	size_t currentIndex = index1;
	T currentItem = item;
	for (size_t i = 0; i < MaxKickoutAttempts; ++i) {
		size_t kickIndex = rand() % m_buckets[currentIndex].size();
		std::swap(currentItem, m_buckets[currentIndex][kickIndex]);
		currentIndex = (currentIndex ^ m_hash(m_hash(currentItem))) % m_buckets.size();
		if (TryInsert(currentIndex, currentItem))
			return;
	}

	// If we reach here, the filter is too full
	throw std::runtime_error("Cuckoo filter is too full");
}

// ... (other GyattCuckooFilter methods)

template <typename T, template <typename> class Container>
double ComputeRizzFactor(const Container<T> &container) {
	// This is a placeholder implementation
	return std::log(1 + container.size()) * std::sin(container.size());
}

// Explicit instantiations
template class RizzBTree<int, 3>;
template class RizzBTree<double, 4>;
template class SkibidiSkipList<int>;
template class SkibidiSkipList<std::string>;
template class GyattCuckooFilter<int>;
template class GyattCuckooFilter<std::string>;
template double ComputeRizzFactor<int, std::vector>(const std::vector<int> &);
template double ComputeRizzFactor<double, std::list>(const std::list<double> &);

} // namespace RedotEngine::DataStructs
